void gradientColor_float(float4 color1, float4 color2, float4 color3, float4 color4, float brightness, float border1, float border2, float border3, out float4 Out)
{
    /*
    descrip: ¬озвращает цвет из градиента на основе €ркости, использу€ 4 базовых цвета.
    param: color1 - базовый цвет дл€ минимальной €ркости.
    param: color2 - базовый цвет дл€ средней €ркости.
    param: color3 - базовый цвет дл€ высокой €ркости.
    param: color4 - базовый цвет дл€ максимальной €ркости.
    param: brightness - значение €ркости в диапазоне [0, 1].
    return: ÷вет из градиента, соответствующий €ркости.
    */
    if (border1<=0){
        border1=0.33;}
    if (border2<=0){
        border2=0.33;}
    if (border3<=0){
        border3=0.34;}

    if (brightness < 0.33) {
        // ћежду color1 и color2
        float t = brightness / border1;
        Out = lerp(color1, color2, t);
    }
    else if (brightness < 0.66) {
        // ћежду color2 и color3
        float t = (brightness - border2) / border2;
        Out = lerp(color2, color3, t);
    }
    else {
        // ћежду color3 и color4
        float t = (brightness - (border1+border2)) / border3;
        Out = lerp(color3, color4, t);
    }
}
