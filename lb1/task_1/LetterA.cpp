#include "LetterA.h"

LetterA::LetterA(sf::Color color, std::unique_ptr<LogicJumping> logic)
    : m_color(color), m_logic(std::move(logic))
{
    CreateShapes();
}

void LetterA::CreateShapes()
{
    float thickness = 20.f;
    float height = 150.f;
    float width = 120.f;

    sf::ConvexShape left;
    left.setPointCount(4);
    left.setPoint(0, { 0, height });
    left.setPoint(1, { thickness, height });
    left.setPoint(2, { width / 2, 0 });
    left.setPoint(3, { width / 2 - thickness, 0 });
    left.setFillColor(m_color);
    m_shapes.push_back(left);

    sf::ConvexShape right;
    right.setPointCount(4);
    right.setPoint(0, { width - thickness, height });
    right.setPoint(1, { width, height });
    right.setPoint(2, { width / 2 + thickness, 0 });
    right.setPoint(3, { width / 2, 0 });
    right.setFillColor(m_color);
    m_shapes.push_back(right);

    m_middle.setSize({ width / 2, thickness });
    m_middle.setFillColor(m_color);
}

void LetterA::UpdateShapePositions()
{
    float x = m_logic->GetX();
    float y = m_logic->GetY();
    float width = 120.f;

    m_shapes[0].setPosition({ x, y });
    m_shapes[1].setPosition({ x, y });

    m_middle.setPosition({ x + width / 4, y + 75.f });
}

void LetterA::Draw(sf::RenderWindow& window)
{
    for (auto& shape : m_shapes)
    {
        window.draw(shape);
    }
    window.draw(m_middle);
}

void LetterA::Update(float deltaTime)
{
    m_logic->Update(deltaTime);
    UpdateShapePositions();
}

void LetterA::StartJump(float velocity)
{
    m_logic->StartJump(velocity);
}

bool LetterA::IsJumping() const
{
    return m_logic->IsJumping();
}