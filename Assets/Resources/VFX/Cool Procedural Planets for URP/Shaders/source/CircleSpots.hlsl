float randomSize(float minSize, float maxSize, int index)
{
    /*
    descrip: Генерирует стабильный детерминированный размер точки в пределах minSize и maxSize, инвертируя размерность для чётных и нечётных индексов.
    param: minSize - минимальный размер точки.
    param: maxSize - максимальный размер точки.
    param: index - порядковый номер точки.
    return: Детерминированный размер точки.
    */

    // Битовые операции для повышения случайности
    int mixedIndex = index ^ (index << 13);
    mixedIndex = mixedIndex ^ (mixedIndex >> 17);
    mixedIndex = mixedIndex ^ (mixedIndex << 5);

    // Преобразуем в диапазон [0, 1]
    float hash = frac(float(mixedIndex) * 0.0000001); // Нормализация для получения дробного значения

    // Инвертируем выборку для чётных и нечётных индексов
    if (index % 2 == 0) {
        // Чётные индексы: ближе к maxSize
        hash = 1.0 - hash;
    }

    // Масштабируем размер в диапазон [minSize, maxSize]
    return lerp(minSize, maxSize, hash);
}





float3 randomPosition(float3 UV, int index, int totalSpots)
{
    /*
    descrip: Генерирует стабильные координаты точки на сфере с равномерным распределением для первых точек.
    param: UV - нормализованные координаты точки.
    param: index - порядковый номер точки.
    param: totalSpots - общее количество точек.
    return: Стабильная позиция точки на сфере.
    */

    // Если это одна из первых четырёх точек, задаём вручную
    if (index == 0) return float3(0.0, 0.0, 1.0); // Северный полюс
    if (index == 1) return float3(0.0, 0.0, -1.0); // Южный полюс
    if (index == 2) return float3(1.0, 0.0, 0.0); // Восток
    if (index == 3) return float3(-1.0, 0.0, 0.0); // Запад

    // Для остальных точек распределяем по сфере
    float goldenRatio = 1.618033988749895; // Золотое сечение
    float angleIncrement = 2.0 * 3.14159265 * goldenRatio;

    // Полярные координаты
    float t = float(index) / float(totalSpots); // Нормализованный индекс [0, 1]
    float spotTheta = acos(1.0 - 2.0 * t);     // Широта
    float spotPhi = float(index) * angleIncrement; // Долгота

    // Преобразуем в декартовы координаты
    return float3(
        sin(spotTheta) * cos(spotPhi),
        sin(spotTheta) * sin(spotPhi),
        cos(spotTheta)
    );
}







void CircleSpots_float(float3 UV, float minSize, float maxSize, int spotCount, float blur, float2 baseOffset, out float Out) 
{
    /*
    descrip: Генерирует точки на сфере с использованием псевдослучайных позиций и размеров.
    param: UV - нормализованные координаты точки на сфере.
    param: minSize - минимальный размер точки.
    param: maxSize - максимальный размер точки.
    param: spotCount - количество точек.
    param: blur - параметр размытия точек.
    param: baseOffset - базовое смещение (x - долгота, y - широта) для вращения точек.
    return: Out - итоговая интенсивность распределения точек.
    */

    float intensity = 0.0;
    float3 normalizedUV = normalize(UV);

    for (int i = 0; i < spotCount; ++i) {
        // Генерируем случайную позицию и размер для текущей точки
        float3 spotPosition = randomPosition(normalizedUV, i,spotCount);
        float spotSize = randomSize(minSize, maxSize, i);

        // Учитываем базовое смещение
        spotPosition = float3(
            sin(baseOffset.y) * spotPosition.x + cos(baseOffset.x) * spotPosition.z,
            spotPosition.y,
            cos(baseOffset.y) * spotPosition.x - sin(baseOffset.x) * spotPosition.z
        );

        // Расстояние от текущей UV точки до центра точки на сфере
        float distToSpot = length(normalizedUV - spotPosition);

        // Добавляем интенсивность с размытой границей
        if (distToSpot < spotSize) {
            float t = distToSpot / spotSize;
            float spotIntensity = exp(-blur * t * t);
            intensity = max(intensity, spotIntensity);
        }
    }

    // Ограничиваем итоговую интенсивность в диапазоне [0, 1]
    Out = saturate(intensity);
}

