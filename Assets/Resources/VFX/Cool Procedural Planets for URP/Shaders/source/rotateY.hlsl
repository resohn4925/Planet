void rotateY_float(float3 UV, float timer, float relaxation, out float3 RotatedUV)
{
    /*
    descrip: ¬ращает UV-координаты вокруг оси Y с ослаблением на полюсах.
    param: UV - нормализованные координаты текстуры.
    param: time - угол вращени€ в радианах.
    param: relaxation - параметр ослаблени€ на полюсах (0 = без изменений, больше 0 = больше расслаблени€).
    return: RotatedUV - координаты UV после вращени€.
    */

    // ѕреобразуем UV в сферические координаты
    float theta = atan2(UV.z, UV.x); // ”гол долготы
    float phi = acos(UV.y);          // ”гол широты

    // ќслабление ст€гивани€ на полюсах
    float relaxedPhi = phi + relaxation * sin(2.0 * phi); // ƒобавл€ем нелинейное смещение

    // ƒобавл€ем вращение по Y
    theta += timer;

    // ѕреобразуем обратно в декартовы координаты
    RotatedUV = float3(
        sin(relaxedPhi) * cos(theta),
        cos(relaxedPhi),
        sin(relaxedPhi) * sin(theta)
    );
}

