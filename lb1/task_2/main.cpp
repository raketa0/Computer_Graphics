#include <SFML/Graphics.hpp>
#include "House.h"

int main()
{
    sf::RenderWindow window(sf::VideoMode({ 800, 600 }), "House");

    sf::Vector2f housePosition(300.f, 250.f);
    House house(housePosition);

    bool isDragging = false;
    sf::Vector2f previousMouse;

    while (window.isOpen())
    {
        while (const std::optional event = window.pollEvent())
        {
            if (event->is<sf::Event::Closed>())
            {
                window.close();
            }

            if (event->is<sf::Event::MouseButtonPressed>())
            {
                sf::Vector2f mousePos = window.mapPixelToCoords(
                    sf::Mouse::getPosition(window));

                if (house.IsClick(mousePos))
                {
                    isDragging = true;
                    previousMouse = mousePos;
                }
            }

            if (event->is<sf::Event::MouseMoved>() && isDragging)
            {
                sf::Vector2f mousePos = window.mapPixelToCoords(
                    sf::Mouse::getPosition(window));

                sf::Vector2f delta = mousePos - previousMouse;
                house.Move(delta);
                previousMouse = mousePos;
            }

            if (event->is<sf::Event::MouseButtonReleased>())
            {
                isDragging = false;
            }
        }

        window.clear(sf::Color::White);
        house.Draw(window);
        window.display();
    }

    return 0;
}
