#include "LogicJumping.h"
#include <cmath>

LogicJumping::LogicJumping(float x, float baseY, float phaseOffset)
    : m_x(x), m_baseY(baseY), m_phaseOffset(phaseOffset)
{
    m_gravity = 600.0f;
    m_time = 0.0f;
    m_isJumping = false;
    m_y = baseY;

    m_jumpStartTime = 0.0f;
    m_initialVelocityY = 0.0f;
    m_baseY = baseY;
}

void LogicJumping::Update(float deltaTime)
{
    m_time += deltaTime;

    if (m_time < m_phaseOffset)
    {
        m_y = m_baseY;
        m_isJumping = false;
        return;
    }

    if (m_isJumping)
    {
        float jumpTime = m_time - m_jumpStartTime;

        //  y = у0 + y0*t + a*t в кв/2
        m_y = m_baseY + m_initialVelocityY * jumpTime + (m_gravity * jumpTime * jumpTime) / 2.0f;

        if (m_y > m_baseY)
        {
            m_y = m_baseY;
            m_isJumping = false;
        }
    }
}

void LogicJumping::StartJump(float velocity)
{
    if (!m_isJumping && m_time >= m_phaseOffset)
    {
        m_initialVelocityY = velocity;
        m_jumpStartTime = m_time;
        m_baseY = m_y;
        m_isJumping = true;
    }
}

bool LogicJumping::IsJumping() const
{
    return m_isJumping;
}

float LogicJumping::GetX() const
{
    return m_x;
}

float LogicJumping::GetY() const
{
    return m_y;
}