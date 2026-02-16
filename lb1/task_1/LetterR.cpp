#include "LetterR.h"

LetterR::LetterR(sf::Color color, std::unique_ptr<LogicJumping> logic)
    :m_color(color), m_logic(std::move(logic))
{
    CreateShapes();
}

void LetterR::CreateShapes()
{
    float thickness = 20.f;
    float height = 150.f;
    float width = 100.f;

    sf::RectangleShape left({ thickness, height });
    left.setFillColor(m_color);
    m_shapes.push_back(left);

    sf::RectangleShape top({ width, thickness });
    top.setFillColor(m_color);
    m_shapes.push_back(top);

    sf::RectangleShape middle({ width, thickness });
    middle.setFillColor(m_color);
    m_shapes.push_back(middle);

    sf::RectangleShape right({ thickness, 50.f });
    right.setFillColor(m_color);
    m_shapes.push_back(right);
}

void LetterR::UpdateShapePositions()
{
    float x = m_logic->GetX();
    float y = m_logic->GetY();
    float width = 100.f;
    float thickness = 20.f;

    m_shapes[0].setPosition({ x, y }); 
    m_shapes[1].setPosition({ x, y });  
    m_shapes[2].setPosition({ x, y + 70.f });            
    m_shapes[3].setPosition({ x + width - thickness, y + thickness });
}

void LetterR::Draw(sf::RenderWindow& window)
{
    for (auto& shape : m_shapes)
        window.draw(shape);
}

void LetterR::Update(float deltaTime)
{
    m_logic->Update(deltaTime);
    UpdateShapePositions();
}

void LetterR::StartJump(float velocity)
{
    m_logic->StartJump(velocity);
}

bool LetterR::IsJumping() const
{
    return m_logic->IsJumping();
}