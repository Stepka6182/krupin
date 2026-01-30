numbers = [10, 20, 30, 40, 50, 60, 70, 80, 90, 100]
print(f"Создан список: {numbers}")
print(f"Длина списка: {len(numbers)} элементов")

len(numbers) >= 10
print(f"Предпоследний элемент: {numbers[-2]}")

print(f"Первый элемент (индекс 2): {numbers[2]}")
print(f"Третий элемент (индекс 3): {numbers[3]}")
print(f"Пятый элемент (индекс 5): {numbers[5]}")

print(f"До изменения: {numbers}")
numbers[3] = 99
print(f"После изменения: {numbers}")

print(f"Элементы с 3-го по 7-й: {numbers[2:7]}")

numbers.append(812)
print(f"После добавления: {numbers}")

middle = len(numbers) // 2
numbers.insert(middle, 555)
print(f"После добавления: (позиция {middle}): {numbers}")

a = 20
count = numbers.count(20)
print(f"Число 20 встречается в списке {count} раз")

numbers_copy = numbers.copy()
print(f"Оригинальный список: {numbers}")
print(f"Копия списка: {numbers_copy}")

if numbers_copy:
    numbers_copy[0] = 999
    numbers_copy[-1] = 777
print(f"Копия после: {numbers_copy}")


print(f"Оригинал: {numbers}")
print(f"Копия: {numbers_copy}")
print(f"Оригинал и копия одинаковы? {numbers == numbers_copy}")
