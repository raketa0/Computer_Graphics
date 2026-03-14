#pragma once
#include <SFML/Graphics.hpp>
#include <optional>
#include <string>

class ImageCanvas
{
public:
    ImageCanvas();

    void Create(unsigned width, unsigned height);
    void Load(const std::string& path);
    void SaveAs(const std::string& path);

    void Draw(sf::RenderWindow& window);
    void HandleEvent(const sf::Event& event, const sf::RenderWindow& window);
    void StopDrawing();

private:
    void UpdateSprite();
    void DrawBrush(sf::Vector2f worldPos);

    sf::Image image;
    sf::Texture texture;
    std::optional<sf::Sprite> sprite;

    bool imageLoaded = false;
    bool drawing = false;

    const unsigned brushSize = 4;

    sf::Vector2f lastPos;
    bool hasLastPos = false;
};