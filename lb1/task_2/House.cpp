#include "House.h"
#include "RectangleShape.h"
#include "TriangleShape.h"
#include "CircleShape.h"
#include "FenceSection.h"

House::House(const sf::Vector2f& position) 
    : m_position(position)
{
	CreateWall();
	CreateRoof();
	CreateDoor();
	CreateWindow();
	CreateFence();
}

void House::Draw(sf::RenderWindow& window)
{
	for (const auto& part : m_parts)
		part->Draw(window);
}

void House::Move(sf::Vector2f& delta)
{
	for (const auto& part : m_parts)
		part->Move(delta);
}

bool House::IsClick(sf::Vector2f& point)
{
	for (const auto& part : m_parts)
	{
		if (part->IsClick(point))
			return true;
	}
	return false;
}

void House::CreateWall()
{
    m_parts.push_back(std::make_unique<RectangleShape>(
        sf::Vector2f(200.f, 150.f), 
        m_position,
        sf::Color(210, 180, 140)
    ));
}

void House::CreateRoof()
{
    sf::Vector2f p1(m_position.x, m_position.y);
    sf::Vector2f p2(m_position.x + 200.f, m_position.y);
    sf::Vector2f p3(m_position.x + 100.f, m_position.y - 100.f);
    m_parts.push_back(std::make_unique<TriangleShape>(p1, p2, p3, sf::Color::Red));

    sf::Vector2f pipePos(m_position.x + 150.f, m_position.y - 80.f);
    m_parts.push_back(std::make_unique<RectangleShape>(sf::Vector2f(20.f, 50.f), pipePos, sf::Color(100, 100, 100)));
}

void House::CreateDoor()
{
    sf::Vector2f doorPos(m_position.x + 80.f, m_position.y + 90.f);
    m_parts.push_back(std::make_unique<RectangleShape>(sf::Vector2f(40.f, 60.f), doorPos, sf::Color(139, 69, 19)));

    sf::Vector2f handlePos(doorPos.x + 30.f, doorPos.y + 30.f);
    m_parts.push_back(std::make_unique<CircleShape>(5.f, handlePos, sf::Color::Black));
}

void House::CreateWindow()
{
    sf::Vector2f windowPos(m_position.x + 30.f, m_position.y + 50.f);
    m_parts.push_back(std::make_unique<RectangleShape>(sf::Vector2f(40.f, 40.f), windowPos, sf::Color::White));
}

void House::CreateFence()
{
    for (int i = 0; i < 8; ++i)
    {
        sf::Vector2f fencePos(m_position.x - 195.f + i * 25.f, m_position.y + 90);
        m_parts.push_back(std::make_unique<FenceSection>(sf::Vector2f(20.f, 60.f), fencePos, sf::Color(139, 69, 19)));
    }
}




