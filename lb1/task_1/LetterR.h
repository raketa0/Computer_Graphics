#pragma once
#include <SFML/Graphics.hpp>
#include <vector>
#include <memory>
#include "ILetter.h"
#include "LogicJumping.h"

class LetterR : public ILetter
{
public:
    LetterR(sf::Color color, std::unique_ptr<LogicJumping> logic);
    void Draw(sf::RenderWindow& window) override;
    void Update(float deltaTime) override;
    void StartJump(float velocity) override;
    bool IsJumping() const override;

private:
    void CreateShapes();
    void UpdateShapePositions();

    sf::Color m_color;
    std::unique_ptr<LogicJumping> m_logic;
    std::vector<sf::RectangleShape> m_shapes;
};