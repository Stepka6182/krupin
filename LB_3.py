import os
import pandas as pd

gg = pd.read_excel(r'C:\Users\23_ИП-291к\Desktop\Книга1.xlsx', sheet_name = ["Лист1", "Лист2", "Лист3"], skiprows= 3)
print(gg["Лист1"].head())
print(gg["Лист2"].head())
print(gg["Лист3"].head())
