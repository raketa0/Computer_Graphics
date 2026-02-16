#include <SFML/Graphics.hpp>
#include "ÑonstructionCircle.h"

int main()
{
    sf::RenderWindow window(sf::VideoMode({ 800, 600 }), "Circle Drawing");

    ConstructionCircle circle(300, sf::Vector2i(400, 300));

    while (window.isOpen())
    {
        while (const std::optional event = window.pollEvent())
        {
            if (event->is<sf::Event::Closed>())
            {
                window.close();
            }
        }

        window.clear(sf::Color::White);
        circle.Draw(window);
        window.display();
    }

    return 0;
}