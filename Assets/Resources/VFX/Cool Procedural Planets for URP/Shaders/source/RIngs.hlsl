void randomRings_float(float2 UV, float blur, float count, float minThickness, float maxThickness, float minRadius, float maxRadius, out float Out)
{
    /*
    descrip: Генерирует кольца разной толщины и псевдослучайной яркости с плавным затуханием на внутренних и внешних радиусах.
    param: UV - координаты текстуры.
    param: blur - степень размытия краёв колец (затухание на внутренних и внешних границах).
    param: count - количество колец.
    param: minThickness - минимальная ширина колец.
    param: maxThickness - максимальная ширина колец.
    param: minRadius - минимальный радиус.
    param: maxRadius - максимальный радиус.
    return: Интенсивность в выходной текстуре, моделирующая кольца.
    */

    // Центрируем UV и вычисляем радиус
    float radius = length(UV * 2.0 - 1.0);

    // Если радиус за пределами диапазона, выходим сразу
    if (radius < minRadius || radius > maxRadius)
    {
        Out = 0.0;
        return;
    }

    // Нормализуем радиус в диапазон [0, 1]
    float normalizedRadius = (radius - minRadius) / (maxRadius - minRadius);

    // Вычисляем индекс текущего кольца
    float ringIndex = normalizedRadius * count;
    int index = int(floor(ringIndex));

    // Псевдослучайная толщина кольца (зависящая от индекса)
    float hashThickness = frac(sin(float(index) * 12.9898) * 43758.5453); // Хэш для толщины
    float thickness = minThickness + hashThickness * (maxThickness - minThickness); // Увеличиваем диапазон

    // Псевдослучайная яркость кольца на основе индекса
    float hashBrightness = frac(sin(float(index) * 34.1234) * 93758.5453); // Хэш для яркости
    float brightness = lerp(0.2, 1.0, hashBrightness); // Диапазон яркости от 0.2 до 1.0

    // Рассчитываем внутренний и внешний радиусы кольца
    float innerRadius = (float(index) / count) * (maxRadius - minRadius) + minRadius;
    float outerRadius = innerRadius + thickness;

    // Создаем кольцо с размытием на внутренних и внешних границах
    float innerBlur = smoothstep(innerRadius - blur, innerRadius, radius);
    float outerBlur = smoothstep(outerRadius, outerRadius + blur, radius);
    Out = (innerBlur - outerBlur) * brightness;
}
