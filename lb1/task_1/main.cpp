#include <SFML/Graphics.hpp>
#include "LetterR.h"
#include "LetterA.h"

int main()
{
    sf::RenderWindow window(
        sf::VideoMode({ 900, 600 }),
        "RAA"
    );


    const float baseLineY = 200.f;

    auto logicR = std::make_unique<LogicJumping>(275.f, baseLineY, 0.0f);
    LetterR letterR(sf::Color::Blue, std::move(logicR));

    auto logicA1 = std::make_unique<LogicJumping>(400.f, baseLineY, 0.4f);
    LetterA letterA1(sf::Color::Green, std::move(logicA1));

    auto logicA2 = std::make_unique<LogicJumping>(550.f, baseLineY, 0.8f);
    LetterA letterA2(sf::Color::Red, std::move(logicA2));

    const float velocityR = -300.f;
    const float velocityA1 = -380.f;
    const float velocityA2 = -240.f;

    auto lastTime = std::chrono::high_resolution_clock::now();

    while (window.isOpen())
    {
        auto currentTime = std::chrono::high_resolution_clock::now();
        float deltaTime = std::chrono::duration<float>(currentTime - lastTime).count();
        lastTime = currentTime;

        while (const std::optional event = window.pollEvent())
        {
            if (event->is<sf::Event::Closed>())
                window.close();
        }

        letterR.Update(deltaTime);
        letterA1.Update(deltaTime);
        letterA2.Update(deltaTime);

        if (!letterR.IsJumping()) 
        {
            letterR.StartJump(velocityR);
        }
        if (!letterA1.IsJumping()) 
        {
            letterA1.StartJump(velocityA1);
        }
        if (!letterA2.IsJumping()) 
        {
            letterA2.StartJump(velocityA2);
        }

        window.clear(sf::Color(20, 20, 20));
        letterR.Draw(window);
        letterA1.Draw(window);
        letterA2.Draw(window);
        window.display();
    }

    return 0;
}
