#include "ImageCanvas.h"
#include <algorithm>

ImageCanvas::ImageCanvas() : imageLoaded(false)
{
}

void ImageCanvas::Load(std::string& path)
{
    if (texture.loadFromFile(path))
    {
        sprite.emplace(texture);
        imageLoaded = true;

        UpdateSprite();
    }
}

void ImageCanvas::Resize(const sf::Vector2u& size)
{
    windowSize = size;
    CreateCheckerboard();

}

void ImageCanvas::UpdateSprite()
{
    if (!sprite || !imageLoaded)
        return;

    auto imageSize = texture.getSize();

    float windowWidth = static_cast<float>(windowSize.x);
    float windowHeight = static_cast<float>(windowSize.y - 30);

    float scaleX = windowWidth / imageSize.x;
    float scaleY = windowHeight / imageSize.y;

    float scale = std::min(scaleX, scaleY);

    if (scale > 1.f)
        scale = 1.f;

    sprite->setScale({ scale, scale });

    float spriteWidth = imageSize.x * scale;
    float spriteHeight = imageSize.y * scale;

    float posX = (windowWidth - spriteWidth) / 2.f;
    float posY = (windowHeight - spriteHeight) / 2.f + 30.f;

    sprite->setPosition({ posX, posY });
}

void ImageCanvas::CreateCheckerboard()
{
    checkerboard.clear();

    for (unsigned y = 30; y < windowSize.y; y += tileSize)
    {
        for (unsigned x = 0; x < windowSize.x; x += tileSize)
        {
            sf::Color tileColor;
            if ((x / tileSize + y / tileSize) % 2 == 0)
            {
                tileColor = sf::Color(200, 200, 200);
            }
            else
            {
                tileColor = sf::Color(150, 150, 150);
            }

            sf::RectangleShape tile(sf::Vector2f(static_cast<float>(tileSize),
                static_cast<float>(tileSize)));
            tile.setPosition(sf::Vector2f(static_cast<float>(x), static_cast<float>(y)));
            tile.setFillColor(tileColor);
            checkerboard.push_back(tile);
        }
    }
}


void ImageCanvas::Draw(sf::RenderWindow& window)
{
    for (const auto& tile : checkerboard)
    {
        window.draw(tile);
    }

    if (imageLoaded && sprite)
    {
        sf::Sprite transparentSprite = *sprite;

        sf::Color spriteColor = sf::Color::White;
        spriteColor.a = 128;
        transparentSprite.setColor(spriteColor);

        window.draw(transparentSprite);
    }
}

void ImageCanvas::HandleEvent(const sf::Event& event, const sf::RenderWindow& window)
{
    if (imageLoaded && sprite)
    {
        dragController.HandleEvent(event, window, *sprite);
    }
}
