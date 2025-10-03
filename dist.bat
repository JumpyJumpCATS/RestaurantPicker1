RMDIR dist /s /q
MD dist
robocopy . dist /E /XD obj bin .git .vs packages TestResults dist /XF .gitignore dist.bat