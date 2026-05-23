#version 330 core

layout(location = 0) in vec2 position;

uniform float progress;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 fragColor;

void main()
{
    float Pi = 3.14159265;
    
    float u = position.x * 2.0 * Pi;
    
    float vSphere = position.y * Pi;        
    float vTorus = position.y * 2.0 * Pi; 

    float x1 = sin(vSphere) * cos(u);
    float y1 = sin(vSphere) * sin(u);
    float z1 = cos(vSphere);

    float R = 0.8;
    float r = 0.35;
    float x2 = (R + r * cos(vTorus)) * cos(u);
    float y2 = (R + r * cos(vTorus)) * sin(u);
    float z2 = r * sin(vTorus);

    vec3 spherePos = vec3(x1, y1, z1);
    vec3 torusPos = vec3(x2, y2, z2);
    vec3 pos = mix(spherePos, torusPos, progress);

    vec3 sphereColor = vec3(0.3, 0.6, 1.0);
    fragColor = mix(sphereColor, sphereColor, progress);

    gl_Position = projection * view * model * vec4(pos, 1.0);
}