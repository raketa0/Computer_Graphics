#include "FileDialog.h"
#include <windows.h>
#include <commdlg.h>
#include <string>

std::string FileDialog::Open()
{
    char filename[MAX_PATH] = "";
    OPENFILENAMEA ofn{};
    ofn.lStructSize = sizeof(ofn);
    ofn.lpstrFilter = "Images\0*.png;\0";
    ofn.lpstrFile = filename;
    ofn.nMaxFile = MAX_PATH;

    if (GetOpenFileNameA(&ofn))
    {
        return filename;
    }

    return "";
}

std::string FileDialog::Save()
{
    char filename[MAX_PATH] = "";
    OPENFILENAMEA ofn{};
    ofn.lStructSize = sizeof(ofn);
    ofn.lpstrFilter = "PNG Image (*.png)\0*.png\0\0";
    ofn.lpstrFile = filename;
    ofn.nMaxFile = MAX_PATH;
    ofn.lpstrDefExt = "png";

    if (GetSaveFileNameA(&ofn))
    {
        std::string result = filename;
        return result;
    }

    return "";
}