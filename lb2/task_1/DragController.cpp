#include "DragController.h"
#include <algorithm>

void DragController::HandleEvent(const sf::Event& event,
    const sf::RenderWindow& window,
    sf::Sprite& sprite)
{
    if (const auto* pressed = event.getIf<sf::Event::MouseButtonPressed>())
    {
        if (pressed->button == sf::Mouse::Button::Left)
        {
            sf::Vector2f mousePos = window.mapPixelToCoords(pressed->position);

            if (sprite.getGlobalBounds().contains(mousePos))
            {
                dragging = true;
                offset = mousePos - sprite.getPosition();
            }
        }
    }

    if (const auto* moved = event.getIf<sf::Event::MouseMoved>())
    {
        if (dragging)
        {
            sf::Vector2f mousePos = window.mapPixelToCoords(moved->position);
            sprite.setPosition(mousePos - offset);

            ClampToWindow(window, sprite);
        }
    }

    if (const auto* released = event.getIf<sf::Event::MouseButtonReleased>())
    {
        if (released->button == sf::Mouse::Button::Left)
        {
            dragging = false;
        }
    }
}

void DragController::ClampToWindow(const sf::RenderWindow& window,
    sf::Sprite& sprite)
{
    auto bounds = sprite.getGlobalBounds();
    auto windowSize = window.getSize();

    float x = sprite.getPosition().x;
    float y = sprite.getPosition().y;

    float maxX = windowSize.x - bounds.size.x;
    float maxY = windowSize.y - bounds.size.y;

    if (x < 0.f)
    {
        x = 0.f;
    }
    else if (x > maxX)
    {
        x = maxX;
    }

    if (y < 30.f)
    {
        y = 30.f;
    }
    else if (y > maxY)
    {
        y = maxY;
    }

    sprite.setPosition({ x, y });
}