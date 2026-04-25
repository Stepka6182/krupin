using System.Net.Http.Json;
using System.Text.Json;

var client = new HttpClient { BaseAddress = new Uri("https://localhost:7112") }; // замените порт на ваш

Console.WriteLine("=== Работа с API списка чисел (аналог ЛР1) ===\n");

// 1. Получить весь список
var allNumbers = await client.GetFromJsonAsync<List<int>>("api/numbers");
Console.WriteLine($"1. Весь список: {string.Join(", ", allNumbers)}");

// 2. Длина списка
var lengthResp = await client.GetFromJsonAsync<JsonElement>("api/numbers/length");
Console.WriteLine($"2. Длина списка: {lengthResp.GetProperty("length")}");

// 3. Элемент по индексу (индекс 2)
var elemByIndex = await client.GetFromJsonAsync<JsonElement>("api/numbers/2");
Console.WriteLine($"3. Элемент с индексом 2: {elemByIndex.GetProperty("value")}");

// 4. Предпоследний элемент
var penultimate = await client.GetFromJsonAsync<JsonElement>("api/numbers/penultimate");
Console.WriteLine($"4. Предпоследний элемент: {penultimate.GetProperty("value")}");

// 5. Срез (с 3-го по 7-й)
var slice = await client.GetFromJsonAsync<List<int>>("api/numbers/slice?start=2&end=7");
Console.WriteLine($"5. Элементы с 3-го по 7-й: {string.Join(", ", slice)}");

// 6. Изменить элемент (индекс 3 на 99)
var putResp = await client.PutAsJsonAsync("api/numbers/3?value=99", new { });
var updated = await putResp.Content.ReadFromJsonAsync<JsonElement>();
Console.WriteLine($"6. Изменение элемента: индекс 3 был {updated.GetProperty("oldValue")}, стал {updated.GetProperty("newValue")}");

// 7. Добавить элемент 812
var postResp = await client.PostAsync("api/numbers?value=812", null);
var added = await postResp.Content.ReadFromJsonAsync<JsonElement>();
Console.WriteLine($"7. Добавлен элемент: {added.GetProperty("added")}, новая длина: {added.GetProperty("newLength")}");

// 8. Вставить 555 в середину
var insertResp = await client.PostAsync("api/numbers/insertmiddle?value=555", null);
var inserted = await insertResp.Content.ReadFromJsonAsync<JsonElement>();
Console.WriteLine($"8. Вставлен {inserted.GetProperty("inserted")} на позицию {inserted.GetProperty("position")}");
Console.WriteLine($"   Список после вставки: {string.Join(", ", inserted.GetProperty("newList").EnumerateArray().Select(x => x.GetInt32()))}");

// 9. Подсчитать вхождения числа 20
var countResp = await client.GetFromJsonAsync<JsonElement>("api/numbers/count/20");
Console.WriteLine($"9. Число 20 встречается {countResp.GetProperty("count")} раз");

// 10. Получить копию
var copy = await client.GetFromJsonAsync<List<int>>("api/numbers/copy");
Console.WriteLine($"10. Копия списка: {string.Join(", ", copy)}");

// 11. Изменить копию
var modifyResp = await client.PostAsync("api/numbers/copy/modify", null);
var modified = await modifyResp.Content.ReadFromJsonAsync<JsonElement>();
var modifiedCopy = modified.GetProperty("modifiedCopy").EnumerateArray().Select(x => x.GetInt32()).ToList();
Console.WriteLine($"11. Копия после изменения: {string.Join(", ", modifiedCopy)}");
Console.WriteLine($"    Оригинал остался: {string.Join(", ", modified.GetProperty("original").EnumerateArray().Select(x => x.GetInt32()))}");

// 12. Сравнить оригинал и копию
var compare = await client.GetFromJsonAsync<JsonElement>("api/numbers/compare");
Console.WriteLine($"12. Оригинал и копия одинаковы? {compare.GetProperty("areEqual")}");