#include "ImageCanvas.h"
#include <algorithm>

ImageCanvas::ImageCanvas() {}

void ImageCanvas::Create(unsigned width, unsigned height)
{
    image = sf::Image({ width, height }, sf::Color::White);
    texture.loadFromImage(image);
    sprite.emplace(texture);
    imageLoaded = true;
    UpdateSprite();
}

void ImageCanvas::Load(const std::string& path)
{
    if (image.loadFromFile(path))
    {
        texture.loadFromImage(image);
        sprite.emplace(texture);
        imageLoaded = true;
        UpdateSprite();
    }
}

void ImageCanvas::SaveAs(const std::string& path)
{
    if (imageLoaded)
    {
        image.saveToFile(path);
    }
}

void ImageCanvas::UpdateSprite()
{
    if (!sprite || !imageLoaded)
    {
        return;
    }

    auto imgSize = image.getSize();

    float scaleX = 1.f;
    float scaleY = 1.f;

    sprite->setScale({ scaleX, scaleY });
    sprite->setPosition({ 0.f, 30.f });
}

void ImageCanvas::DrawBrush(sf::Vector2f worldPos)
{
    if (!sprite) 
    {
        return;
    }

    sf::Vector2f sp = sprite->getPosition();
    int x = worldPos.x - sp.x;
    int y = worldPos.y - sp.y;

    if (x < 0 || y < 0 || x >= image.getSize().x || y >= image.getSize().y)
    {
        return;
    }

    image.setPixel({ static_cast<unsigned>(x), static_cast<unsigned>(y) }, sf::Color::Black);
                 
    texture.update(image);
}

void ImageCanvas::HandleEvent(const sf::Event& event, const sf::RenderWindow& window)
{
    if (!imageLoaded || !sprite) 
    {
        return;
    }

    if (const auto* press = event.getIf<sf::Event::MouseButtonPressed>())
    {
        if (press->button == sf::Mouse::Button::Left)
        {
            drawing = true;
            lastPos = window.mapPixelToCoords({ press->position.x, press->position.y });
            hasLastPos = true;
            DrawBrush(lastPos);
        }
    }

    if (const auto* move = event.getIf<sf::Event::MouseMoved>())
    {
        sf::Vector2f pos = window.mapPixelToCoords({ move->position.x, move->position.y });

        if (drawing && hasLastPos)
        {
            float dx = pos.x - lastPos.x;
            float dy = pos.y - lastPos.y;
            int steps = std::max(std::abs(dx), std::abs(dy));
            for (int i = 0; i <= steps; i++)
            {
                sf::Vector2f interp = lastPos + sf::Vector2f(dx * i / float(steps), dy * i / float(steps));
                DrawBrush(interp);
            }
        }

        lastPos = pos;
        hasLastPos = true;
    }

    if (const auto* release = event.getIf<sf::Event::MouseButtonReleased>())
    {
        if (release->button == sf::Mouse::Button::Left)
        {
            drawing = false;
            hasLastPos = false;
        }
    }
}

void ImageCanvas::StopDrawing()
{
    drawing = false;
    hasLastPos = false;
}

void ImageCanvas::Draw(sf::RenderWindow& window)
{
    if (sprite)
    {
        window.draw(*sprite);
    }
}