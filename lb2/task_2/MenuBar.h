#pragma once
#include <SFML/Graphics.hpp>

enum class MenuAction
{
    NONE,
    NEW_FILE,
    OPEN_FILE,
    SAVE_AS
};

class MenuBar
{
public:
    MenuBar();

    MenuAction HandleEvent(const sf::Event& event, const sf::RenderWindow& window);
    void Draw(sf::RenderWindow& window);

private:
    sf::Font font;
    sf::RectangleShape menuBar;

    sf::Text fileText;
    sf::RectangleShape dropDown;

    sf::Text newText;
    sf::Text openText;
    sf::Text saveText;

    bool showDropdown = false;

    const float height = 30.f;
};