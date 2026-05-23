#version 330 core

out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoord;

uniform sampler2D tex;
uniform vec3 lightPos;

void main()
{
    vec3 norm = normalize(Normal);

    vec3 lightDir =
        normalize(lightPos - FragPos);

    float diff =
        max(dot(norm, lightDir), 0.35);

    vec4 textureColor =
        texture(tex, TexCoord);

    FragColor =
        vec4(textureColor.rgb * diff, 1.0);
}