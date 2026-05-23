#version 330 core

layout(location = 0) in float aX;

uniform float scale;

#define PI 3.141592653589793

void main()
{
    float x = aX;

    float R = (1.0 + sin(x))
         * (1.0 + 0.9 * cos(8.0 * x))
         * (1.0 + 0.1 * cos(24.0 * x))
         * (0.5 + 0.05 * cos(140.0 * x));

    float xp = R * cos(x);
    float yp = R * sin(x);

    gl_Position = vec4(xp * scale, yp * scale, 0.0, 1.0);
}