using UnityEngine;

public static class AnimationCurveExtensionMethods
{
    public static (float time, float value) GetGlobalMaximum(this AnimationCurve curve)
    {
        (_, float maxTime) = CalculateGlobalMinMaxTimes(curve);
        return (maxTime, curve.Evaluate(maxTime));
    }

    public static (float time, float value) GetGlobalMinimum(this AnimationCurve curve)
    {
        (float minTime, _) = CalculateGlobalMinMaxTimes(curve);
        return (minTime, curve.Evaluate(minTime));
    }

    private static (float minTime, float maxTime) CalculateLocalMinMaxTimes(float y0, float y1, float y2, float y3)
    {
        // Função da primeira derivada da curva de Bezier cúbica
        // f'(t) = 3 * (1-t)^2 * (y1 - y0) + 6 * (1-t) * t * (y2 - y1) + 3 * t^2 * (y3 - y2)

        // Método de Newton-Raphson para encontrar as raízes
        const float epsilon = 0.001f; // Precisão desejada
        const int maxIterations = 1000; // Número máximo de iterações
        float t = .5f; // Valor inicial de t

        for (int i = 0; i < maxIterations; i++)
        {
            // Avaliar a primeira derivada da curva de Bezier cúbica em t
            float derivative = 3 * Mathf.Pow(1 - t, 2) * (y1 - y0) +
                               6 * (1 - t) * t * (y2 - y1) +
                               3 * Mathf.Pow(t, 2) * (y3 - y2);

            // Se a derivada for pequena o suficiente, consideramos t como uma raiz
            if (Mathf.Abs(derivative) < epsilon)
                break;

            // Atualizar o valor de t usando o método de Newton-Raphson
            t = t - ((3 * Mathf.Pow(1 - t, 2) * (y1 - y0) + 6 * (1 - t) * t * (y2 - y1) + 3 * Mathf.Pow(t, 2) * (y3 - y2)) /
                     (6 * (1 - t) * (y2 - 2 * y1 + y0) + 3 * (2 * y1 - 2 * y0) + 6 * t * (y3 - 2 * y2 + y1)));
        }

        // Calcular os valores de tempo correspondentes
        float minTime = Mathf.Clamp01(t - epsilon);
        float maxTime = Mathf.Clamp01(t + epsilon);

        return (minTime, maxTime);
    }

    private static (float minTime, float maxTime) CalculateGlobalMinMaxTimes(AnimationCurve curve)
    {
        Keyframe[] keys = curve.keys;

        float minTime = float.MaxValue;
        float maxTime = float.MinValue;

        for (int i = 0; i < keys.Length; i++)
        {
            float y0 = keys[i].value;

            if (i < keys.Length - 1)
            {
                float y1 = keys[i + 1].value;
                float y2 = keys[i].outTangent;
                float y3 = keys[i + 1].inTangent;

                (float localMinimum, float localMaximum) = CalculateLocalMinMaxTimes(y0, y1, y2, y3);

                minTime = Mathf.Min(minTime, localMinimum);
                maxTime = Mathf.Max(maxTime, localMaximum);
            }
        }

        return (minTime, maxTime);
    }
}