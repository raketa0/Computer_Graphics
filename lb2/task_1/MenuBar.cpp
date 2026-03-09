#include "MenuBar.h"

MenuBar::MenuBar()
    : font("Arial.ttf"),
    fileText(font, "File", 18),
    openText(font, "Open", 18),
    showDropdown(false)
{
    menuBar.setFillColor(sf::Color(50, 50, 50));

    fileText.setFont(font);
    fileText.setString("File");
    fileText.setCharacterSize(18);
    fileText.setFillColor(sf::Color::White);
    fileText.setPosition(sf::Vector2f(10.f, 5.f));

    dropDownMenu.setSize(sf::Vector2f(dropDownWidth, 30.f));
    dropDownMenu.setFillColor(sf::Color(70, 70, 70));
    dropDownMenu.setOutlineColor(sf::Color(100, 100, 100));
    dropDownMenu.setOutlineThickness(1.f);

    openText.setFont(font);
    openText.setString("Open");
    openText.setCharacterSize(16);
    openText.setFillColor(sf::Color::White);
    openText.setPosition(sf::Vector2f(10.f, menuBarHeight + 5.f));
}

void MenuBar::UpdateSize(const sf::Vector2u& windowSize)
{
    menuBar.setSize(sf::Vector2f(static_cast<float>(windowSize.x), menuBarHeight));
    dropDownMenu.setPosition(sf::Vector2f(0.f, menuBarHeight));
} 

bool MenuBar::HandleEvent(const sf::Event& event, const sf::RenderWindow& window)
{
    if (const auto* mousePressed = event.getIf<sf::Event::MouseButtonPressed>())
    {
        if (mousePressed->button == sf::Mouse::Button::Left)
        {
            sf::Vector2f mousePos = window.mapPixelToCoords(mousePressed->position);

            if (fileText.getGlobalBounds().contains(mousePos))
            {
                showDropdown = !showDropdown;
                return false;
            }

            if (showDropdown)
            {
                if (openText.getGlobalBounds().contains(mousePos))
                {
                    showDropdown = false;
                    return true;
                }
                else if (!dropDownMenu.getGlobalBounds().contains(mousePos))
                {
                    showDropdown = false;
                }
            }
        }
    }

    return false;
}

void MenuBar::Draw(sf::RenderWindow& window)
{
    window.draw(menuBar);
    window.draw(fileText);

    if (showDropdown)
    {
        window.draw(dropDownMenu);
        window.draw(openText);
    }
}