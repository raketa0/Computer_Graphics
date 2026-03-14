#include <SFML/Graphics.hpp>
#include "MenuBar.h"
#include "ImageCanvas.h"
#include "FileDialog.h"

int main()
{
    sf::RenderWindow window(sf::VideoMode({ 800, 600 }), "Image");

    MenuBar menu;
    ImageCanvas canvas;

    canvas.Create(800, 570);

    while (window.isOpen())
    {
        while (auto optionalEvent = window.pollEvent())
        {

            const sf::Event& event = optionalEvent.value();

            if (event.is<sf::Event::Closed>())
            {
                window.close();
            }

            canvas.HandleEvent(event, window);

            MenuAction action = menu.HandleEvent(event, window);

            if (action == MenuAction::NEW_FILE)
            {
                canvas.Create(800, 570);
            }

            if (action == MenuAction::OPEN_FILE)
            {
                canvas.StopDrawing();
                auto path = FileDialog::Open();
                if (!path.empty())
                {
                    canvas.Load(path);
                }
            }

            if (action == MenuAction::SAVE_AS)
            {
                canvas.StopDrawing();

                auto path = FileDialog::Save();
                if (!path.empty())
                {
                    canvas.SaveAs(path);
                }
            }
        }

        window.clear(sf::Color::White);
        canvas.Draw(window);
        menu.Draw(window);
        window.display();
    }
}