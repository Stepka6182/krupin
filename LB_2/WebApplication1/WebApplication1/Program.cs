using System.Collections.Generic;

// Создаём список чисел, как в ЛР1
var numbers = new List<int> { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
Console.WriteLine($"Создан список: {string.Join(", ", numbers)}");
Console.WriteLine($"Длина списка: {numbers.Count} элементов");

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// 1. Получить весь список (GET /api/numbers)
app.MapGet("/api/numbers", () => numbers);

// 2. Получить длину списка (GET /api/numbers/length)
app.MapGet("/api/numbers/length", () => new { length = numbers.Count });

// 3. Получить элемент по индексу (GET /api/numbers/{index})
app.MapGet("/api/numbers/{index}", (int index) =>
{
    if (index < 0 || index >= numbers.Count)
        return Results.NotFound(new { message = "Индекс вне диапазона" });
    return Results.Json(new { index, value = numbers[index] });
});

// 4. Получить предпоследний элемент (GET /api/numbers/penultimate)
app.MapGet("/api/numbers/penultimate", () =>
{
    if (numbers.Count < 2)
        return Results.NotFound(new { message = "Недостаточно элементов" });
    return Results.Json(new { value = numbers[^2] });
});

// 5. Получить срез (GET /api/numbers/slice?start=2&end=7)
app.MapGet("/api/numbers/slice", (int start, int end) =>
{
    if (start < 0 || end > numbers.Count || start >= end)
        return Results.BadRequest(new { message = "Некорректные границы" });
    var slice = numbers[start..end];
    return Results.Json(slice);
});

// 6. Изменить элемент по индексу (PUT /api/numbers/{index}?value=99)
app.MapPut("/api/numbers/{index}", (int index, int value) =>
{
    if (index < 0 || index >= numbers.Count)
        return Results.NotFound(new { message = "Индекс вне диапазона" });
    int oldValue = numbers[index];
    numbers[index] = value;
    return Results.Json(new { index, oldValue, newValue = value });
});

// 7. Добавить элемент в конец (POST /api/numbers?value=812)
app.MapPost("/api/numbers", (int value) =>
{
    numbers.Add(value);
    return Results.Json(new { added = value, newLength = numbers.Count });
});

// 8. Вставить элемент в середину (POST /api/numbers/insertmiddle?value=555)
app.MapPost("/api/numbers/insertmiddle", (int value) =>
{
    int middle = numbers.Count / 2;
    numbers.Insert(middle, value);
    return Results.Json(new { inserted = value, position = middle, newList = numbers });
});

// 9. Подсчитать вхождения числа (GET /api/numbers/count/{value})
app.MapGet("/api/numbers/count/{value}", (int value) =>
{
    int count = numbers.Count(n => n == value);
    return Results.Json(new { value, count });
});

// 10. Получить копию списка (GET /api/numbers/copy)
app.MapGet("/api/numbers/copy", () =>
{
    var copy = new List<int>(numbers);
    return Results.Json(copy);
});

// 11. Изменить копию (POST /api/numbers/copy/modify)
app.MapPost("/api/numbers/copy/modify", () =>
{
    var copy = new List<int>(numbers);
    if (copy.Count > 0)
    {
        copy[0] = 999;
        copy[^1] = 777;
    }
    return Results.Json(new { modifiedCopy = copy, original = numbers });
});

// 12. Сравнить оригинал и копию (GET /api/numbers/compare)
app.MapGet("/api/numbers/compare", () =>
{
    var copy = new List<int>(numbers);
    bool areEqual = numbers.SequenceEqual(copy);
    return Results.Json(new { areEqual, original = numbers, copy = copy });
});

app.Run();