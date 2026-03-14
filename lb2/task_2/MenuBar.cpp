#include "MenuBar.h"

MenuBar::MenuBar()
    : font("Arial.ttf"),               
    fileText(font, "File", 18),
    newText(font, "New", 18),
    openText(font, "Open", 18),
    saveText(font, "Save As", 18),
    showDropdown(false)
{

    fileText.setFillColor(sf::Color::Black);
    newText.setFillColor(sf::Color::Black);
    openText.setFillColor(sf::Color::Black);
    saveText.setFillColor(sf::Color::Black);

    fileText.setPosition({ 10.f, 3.f });

    float menuHeight = 30.f;
    dropDown.setSize({ 150.f, 90.f });
    dropDown.setFillColor(sf::Color(240, 240, 240));
    dropDown.setPosition({ 0.f, menuHeight });

    newText.setPosition({ 10.f, menuHeight + 5.f });
    openText.setPosition({ 10.f, menuHeight + 30.f });
    saveText.setPosition({ 10.f, menuHeight + 55.f });

    menuBar.setSize({ 800.f, menuHeight });
    menuBar.setFillColor(sf::Color(220, 220, 220));
}

MenuAction MenuBar::HandleEvent(const sf::Event& event, const sf::RenderWindow& window)
{
    if (const auto* pressed = event.getIf<sf::Event::MouseButtonPressed>())
    {
        sf::Vector2f pos = window.mapPixelToCoords(pressed->position);

        if (fileText.getGlobalBounds().contains(pos))
        {
            showDropdown = !showDropdown;
            return MenuAction::NONE;
        }

        if (showDropdown)
        {
            if (newText.getGlobalBounds().contains(pos))
            {
                showDropdown = false;
                return MenuAction::NEW_FILE;
            }
            if (openText.getGlobalBounds().contains(pos))
            {
                showDropdown = false;
                return MenuAction::OPEN_FILE;
            }
            if (saveText.getGlobalBounds().contains(pos))
            {
                showDropdown = false;
                return MenuAction::SAVE_AS;
            }
        }
    }
    return MenuAction::NONE;
}

void MenuBar::Draw(sf::RenderWindow& window)
{
    window.draw(menuBar);
    window.draw(fileText);

    if (showDropdown)
    {
        window.draw(dropDown);
        window.draw(newText);
        window.draw(openText);
        window.draw(saveText);
    }
}