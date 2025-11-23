// Функция эквалайзера для кастомной ноды
void Equalize_float(float value, float minValue, float maxValue, out float Out)
{
    // Нормализуем значение в диапазоне [0, 1]
    float normalizedValue = (value - minValue) / (maxValue - minValue);
    
    // Применяем гамма-коррекцию для равномерного распределения
    Out = pow(normalizedValue, 1.0); // В данном примере гамма-коррекция равна 1.0, но можно добавить параметр для гибкости
}
