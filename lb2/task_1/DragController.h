#pragma once
#include <SFML/Graphics.hpp>

class DragController
{
public:
    void HandleEvent(const sf::Event& event,
        const sf::RenderWindow& window,
        sf::Sprite& sprite);

private:
    void ClampToWindow(const sf::RenderWindow& window,
        sf::Sprite& sprite);

    bool dragging = false;
    sf::Vector2f offset;
};