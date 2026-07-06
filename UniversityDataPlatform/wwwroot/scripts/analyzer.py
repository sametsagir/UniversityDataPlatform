import sys
import subprocess

try:
    import openpyxl
except ImportError:
    try:
        subprocess.check_call([sys.executable, "-m", "pip", "install", "openpyxl"])
    except Exception as e:
        pass

import pandas as pd
import json
import numpy as np
import io
import base64
import matplotlib.pyplot as plt
import seaborn as sns
from sklearn.model_selection import train_test_split
from sklearn.tree import DecisionTreeClassifier, DecisionTreeRegressor
from sklearn.ensemble import RandomForestClassifier, RandomForestRegressor, ExtraTreesClassifier
from sklearn.linear_model import LogisticRegression, Ridge, ElasticNet
from sklearn.preprocessing import LabelEncoder
from sklearn.metrics import accuracy_score, r2_score, confusion_matrix

# --- GLOBAL PLOT SETTINGS (Optimize Readability and Dimensions) ---
plt.style.use('ggplot')
sns.set_theme(style="whitegrid")
plt.rcParams.update({
    'font.size': 12,          
    'axes.titlesize': 16,     
    'axes.labelsize': 14,     
    'xtick.labelsize': 12,    
    'ytick.labelsize': 12,    
    'legend.fontsize': 12,    
    'figure.titlesize': 18    
})

def get_plot_base64():
    """Converts the plot to a high-resolution Base64 string"""
    buf = io.BytesIO()
    plt.savefig(buf, format='png', bbox_inches='tight', dpi=120)
    plt.close()
    return base64.b64encode(buf.getvalue()).decode('utf-8')

def analyze_dataset(file_path, target_col=None, excluded_cols=[]):
    try:
        if file_path.lower().endswith(('.xlsx', '.xls')):
            try:
                raw_df = pd.read_excel(file_path)
            except Exception:
                raw_df = pd.read_excel(file_path, engine='openpyxl')
        else:
            try:
                raw_df = pd.read_csv(file_path, sep=None, engine='python')
            except Exception:
                raw_df = pd.read_csv(file_path)
        original_columns = raw_df.columns.tolist()
        missing_counts = raw_df.isnull().sum().to_dict()
        dtypes_dict = raw_df.dtypes.to_dict()

        # If target is not specified or not in file, select the last column by default
        if target_col is None or target_col not in raw_df.columns:
            target_col_name = raw_df.columns[-1]
        else:
            target_col_name = target_col

        # Filter columns to exclude (never exclude target column)
        cols_to_drop = [col for col in excluded_cols if col in raw_df.columns and col != target_col_name]

        df = raw_df.copy()
        if cols_to_drop:
            df = df.drop(columns=cols_to_drop)

        if len(df) > 2000:
            df = df.sample(n=2000, random_state=42)
        
        df = df.dropna()

        # Convert categorical columns to numeric values
        le = LabelEncoder()
        for col in df.select_dtypes(include=['object']).columns:
            df[col] = le.fit_transform(df[col].astype(str))

        # Define features and target
        y = df[target_col_name]
        X = df.drop(columns=[target_col_name])
        
        # Task Detection: If target variable is categorical or has few unique numeric values (< 15), do classification
        is_numeric = pd.api.types.is_numeric_dtype(y.dtype)
        if not is_numeric:
            is_classification = True
        else:
            is_classification = y.nunique() < 15
        task_type = "Classification" if is_classification else "Regression"
        
        X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
        performance = []
        
        best_model = None
        best_score = -1
        best_pred = None
        
        # Model Selection
        if is_classification:
            models = {
                "Random Forest": RandomForestClassifier(n_estimators=50, max_depth=8, n_jobs=-1),
                "Decision Tree": DecisionTreeClassifier(max_depth=8),
                "Logistic Regression": LogisticRegression(max_iter=500)
            }
        else:
            models = {
                "Ridge Regression": Ridge(alpha=1.0),
                "Decision Tree": DecisionTreeRegressor(max_depth=8),
                "Random Forest": RandomForestRegressor(n_estimators=50, max_depth=8, n_jobs=-1)
            }

        # Training and Evaluation
        for name, model in models.items():
            model.fit(X_train, y_train)
            y_pred = model.predict(X_test)
            if is_classification:
                score = round(accuracy_score(y_test, y_pred) * 100, 2)
            else:
                score = round(max(0, r2_score(y_test, y_pred)) * 100, 2)
            
            performance.append({"model_name": name, "score": score})
            
            if score > best_score:
                best_score = score
                best_model = model
                best_pred = y_pred

        # --- PLOT GENERATION ---
        charts = {}

        # 1. COMMON PLOT: Feature Importance (All columns)
        fig_height = max(6, len(X.columns) * 0.4)
        plt.figure(figsize=(12, fig_height))
        
        if hasattr(best_model, 'feature_importances_'):
            importances = best_model.feature_importances_
        elif hasattr(best_model, 'coef_'):
            importances = np.abs(best_model.coef_)
            if importances.ndim > 1: importances = np.mean(importances, axis=0)
        else:
            importances = np.zeros(len(X.columns))

        feat_importances = pd.Series(importances, index=X.columns)
        feat_importances.sort_values(ascending=True).plot(kind='barh', color='skyblue')
        plt.title('Feature Importance Analysis')
        plt.xlabel('Importance Score')
        charts['feature_importance'] = get_plot_base64()

        if is_classification:
            # 2. Confusion Matrix
            plt.figure(figsize=(10, 8))
            cm = confusion_matrix(y_test, best_pred)
            sns.heatmap(cm, annot=True, fmt='d', cmap='Blues', annot_kws={"size": 14})
            plt.title('Model Confusion Matrix')
            plt.ylabel('Actual Class')
            plt.xlabel('Predicted Class')
            charts['confusion_matrix'] = get_plot_base64()

            # 3. Target Distribution (Pie Chart)
            plt.figure(figsize=(10, 8))
            raw_df[target_col_name].value_counts().plot(
                kind='pie', 
                autopct='%1.1f%%', 
                colors=sns.color_palette('pastel'),
                startangle=140
            )
            plt.title(f'Target Variable Distribution ({target_col_name})')
            plt.ylabel('') 
            charts['target_distribution'] = get_plot_base64()
        else:
            # 2. Prediction vs Actual
            plt.figure(figsize=(10, 8))
            plt.scatter(y_test, best_pred, alpha=0.6, color='orange', s=60) 
            plt.plot([y_test.min(), y_test.max()], [y_test.min(), y_test.max()], 'k--', lw=2.5)
            plt.title('Actual vs Predicted Values')
            plt.xlabel('Actual Values')
            plt.ylabel('Predicted Values')
            charts['prediction_vs_actual'] = get_plot_base64()

            # 3. Residuals Distribution
            plt.figure(figsize=(10, 8))
            sns.histplot(y_test - best_pred, kde=True, color='green', bins=20)
            plt.title('Residuals (Error) Distribution')
            plt.xlabel('Error Magnitude')
            plt.ylabel('Frequency')
            charts['residuals_distribution'] = get_plot_base64()

        # Variables list (For Blazor table)
        variables_list = []
        for col in original_columns:
            role = "Target" if col == target_col_name else ("Excluded" if col in cols_to_drop else "Feature")
            col_dtype = dtypes_dict[col]
            v_type = "Numeric" if pd.api.types.is_numeric_dtype(col_dtype) else "Categorical"
            variables_list.append({
                "name": col,
                "role": role,
                "type": v_type,
                "missing": str(missing_counts[col])
            })

        results = {
            "instances": len(raw_df),
            "features": len(X.columns),
            "task_type": task_type,
            "performance": performance,
            "variables": variables_list,
            "charts": charts
        }
        print(json.dumps(results))

    except Exception as e:
        print(json.dumps({
            "error": str(e), 
            "task_type": "Error", 
            "performance": [], 
            "variables": [], 
            "charts": {}
        }))

if __name__ == "__main__":
    if len(sys.argv) > 1:
        file_path = sys.argv[1]
        target_col = sys.argv[2] if len(sys.argv) > 2 and sys.argv[2] else None
        excluded_cols = sys.argv[3].split(",") if len(sys.argv) > 3 and sys.argv[3] else []
        analyze_dataset(file_path, target_col, excluded_cols)