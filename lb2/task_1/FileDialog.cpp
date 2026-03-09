#include "FileDialog.h"
#include <windows.h>
#include <commdlg.h>

std::string FileDialog::Open()
{
    char filename[MAX_PATH] = "";

    OPENFILENAMEA ofn = {};
    ofn.lStructSize = sizeof(ofn);
    ofn.lpstrFilter = "Images\0*.png\0";
    ofn.lpstrFile = filename;
    ofn.nMaxFile = MAX_PATH;

    if (GetOpenFileNameA(&ofn))
        return filename;

    return "";
}
