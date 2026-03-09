#pragma once
#include <SFML/Graphics.hpp>

class MenuBar
{
public:
    MenuBar();

    bool HandleEvent(const sf::Event& event, const sf::RenderWindow& window);
    void Draw(sf::RenderWindow& window);
    void UpdateSize(const sf::Vector2u& windowSize);

private:
    sf::Font font;
    sf::RectangleShape menuBar;
    sf::Text fileText;
    sf::RectangleShape dropDownMenu;
    sf::Text openText;
    bool showDropdown;

    float menuBarHeight = 30.f;
    float dropDownWidth = 150.f;
};