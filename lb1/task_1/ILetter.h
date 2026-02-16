#pragma once
#include <SFML/Graphics.hpp>

class ILetter
{
public:
    virtual void Draw(sf::RenderWindow& window) = 0;
    virtual void Update(float deltaTime) = 0;
    virtual void StartJump(float velocity) = 0;
    virtual bool IsJumping() const = 0;
    virtual ~ILetter() = default;
};