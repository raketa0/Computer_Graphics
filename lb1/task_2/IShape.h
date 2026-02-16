#pragma once

#include <SFML/Graphics.hpp>

class IShape
{
public:
    virtual void Draw(sf::RenderWindow& window) = 0;
    virtual void Move(sf::Vector2f& delta) = 0;
    virtual bool IsClick(sf::Vector2f& point) = 0;
};
