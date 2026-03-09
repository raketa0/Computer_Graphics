#include <SFML/Graphics.hpp>
#include "MenuBar.h"
#include "ImageCanvas.h"
#include "FileDialog.h"

int main()
{
    sf::RenderWindow window(sf::VideoMode({ 900, 600 }), "Image");

    MenuBar menu;
    ImageCanvas canvas;

    canvas.Resize(window.getSize());
    menu.UpdateSize(window.getSize());

    while (window.isOpen())
    {
        while (auto event = window.pollEvent())
        {
            if (event->is<sf::Event::Closed>())
                window.close();

            if (const auto* resized =
                event->getIf<sf::Event::Resized>())
            {
                sf::FloatRect visibleArea(
                    { 0.f,0.f },
                    { static_cast<float>(resized->size.x),
                     static_cast<float>(resized->size.y) });

                window.setView(sf::View(visibleArea));

                sf::Vector2u newSize(
                    resized->size.x,
                    resized->size.y);

                canvas.Resize(newSize);
                menu.UpdateSize(newSize);
            }

            canvas.HandleEvent(*event, window);

            if (menu.HandleEvent(*event, window))
            {
                std::string path = FileDialog::Open();

                if (!path.empty())
                    canvas.Load(path);
            }
        }

        window.clear();

        canvas.Draw(window);
        menu.Draw(window);

        window.display();
    }

    return 0;
}