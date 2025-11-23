void Stripes_float(float2 UV, float blur, float tiling, float minThickness, float maxThickness, out float Out)
{
    /*
    descrip: —оздаЄт полосы на сфере с уникальной шириной дл€ каждой полосы в заданных пределах.
    param: UV - координаты текстуры.
    param: blur - степень размыти€ краЄв полос.
    param: tiling - количество полос (плитка по вертикали).
    param: minThickness - минимальна€ ширина полос.
    param: maxThickness - максимальна€ ширина полос.
    return: »нтенсивность полос в выходной текстуре.
    */

    // ћасштабируем UV дл€ управлени€ количеством полос
    float stripedY = UV.y * tiling;

    // ¬ычисл€ем индекс текущей полосы
    int index = int(floor(stripedY));

    // √енераци€ случайной ширины полосы
    int mixedIndex = index * 3287; // ”множаем на произвольное число дл€ большей уникальности
    mixedIndex = mixedIndex ^ (mixedIndex << 13);
    mixedIndex = mixedIndex ^ (mixedIndex >> 17);
    mixedIndex = mixedIndex ^ (mixedIndex << 5);
    float hash = frac(float(mixedIndex) * 0.0000001); // Ќормализаци€ в диапазон [0, 1]
    float thickness = lerp(minThickness, maxThickness, hash);

    // √енераци€ базового сигнала полос через frac
    float baseStripes = frac(stripedY);

    // ќпредел€ем центр полосы
    float stripeCenter = 0.5;

    // ѕримен€ем размытие и ширину полос
    Out = smoothstep(stripeCenter - thickness - blur, stripeCenter - thickness, baseStripes) - 
          smoothstep(stripeCenter + thickness, stripeCenter + thickness + blur, baseStripes);
}

