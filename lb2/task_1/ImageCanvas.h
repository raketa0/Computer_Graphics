#pragma once
#include <SFML/Graphics.hpp>
#include <optional>
#include <vector>
#include <string>
#include "DragController.h"

class ImageCanvas
{
public:
    ImageCanvas();

    void Load( std::string& path);
    void Resize(const sf::Vector2u& size);
    void Draw(sf::RenderWindow& window);
    void HandleEvent(const sf::Event& event, const sf::RenderWindow& window);

private:
    void UpdateSprite();
    void CreateCheckerboard();

    sf::Texture texture;
    std::optional<sf::Sprite> sprite;
    std::vector<sf::RectangleShape> checkerboard;

    bool imageLoaded = false;
    sf::Vector2u windowSize;
    const int tileSize = 20;
    DragController dragController;

};