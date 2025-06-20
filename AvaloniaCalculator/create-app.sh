
# Имя проекта
APP_NAME="AvaloniaCalculator"
# Целевая папка публикации
FRAMEWORK="net9.0"
RUNTIME="osx-x64"
CONFIG="Release"
PUBLISH_DIR="bin/$CONFIG/$FRAMEWORK/$RUNTIME/publish"
ICON_PNG="icon.png"
ICONSET_DIR="tmp.iconset"
ICON_ICNS="$APP_NAME.icns"

# 1. Публикация проекта
echo "🛠 Публикация проекта..."
dotnet publish -c $CONFIG -r $RUNTIME --self-contained true /p:PublishSingleFile=true \
/p:PublishReadyToRun=true \
/p:PublishStripSymbols=true \
/p:PublishTrimmed=true \
 /p:TrimMode=link


# 2. Создание .icns из .png
echo "🎨 Генерация .icns из $ICON_PNG..."
rm -rf "$ICONSET_DIR"
mkdir "$ICONSET_DIR"
sips -z 16 16     "$ICON_PNG" --out "$ICONSET_DIR/icon_16x16.png"
sips -z 32 32     "$ICON_PNG" --out "$ICONSET_DIR/icon_16x16@2x.png"
sips -z 32 32     "$ICON_PNG" --out "$ICONSET_DIR/icon_32x32.png"
sips -z 64 64     "$ICON_PNG" --out "$ICONSET_DIR/icon_32x32@2x.png"
sips -z 128 128   "$ICON_PNG" --out "$ICONSET_DIR/icon_128x128.png"
sips -z 256 256   "$ICON_PNG" --out "$ICONSET_DIR/icon_128x128@2x.png"
sips -z 256 256   "$ICON_PNG" --out "$ICONSET_DIR/icon_256x256.png"
sips -z 512 512   "$ICON_PNG" --out "$ICONSET_DIR/icon_256x256@2x.png"
sips -z 512 512   "$ICON_PNG" --out "$ICONSET_DIR/icon_512x512.png"
cp "$ICON_PNG" "$ICONSET_DIR/icon_512x512@2x.png"

iconutil -c icns "$ICONSET_DIR" -o "$ICON_ICNS"
rm -rf "$ICONSET_DIR"

# 2. Создание структуры .app
echo "📁 Создание .app обёртки..."
APP_BUNDLE="$APP_NAME.app"
APP_MACOS_DIR="$APP_BUNDLE/Contents/MacOS"
APP_RESOURCES_DIR="$APP_BUNDLE/Contents/Resources"
rm -rf "$APP_BUNDLE"
mkdir -p "$APP_MACOS_DIR"
mkdir -p "$APP_RESOURCES_DIR"

# 3. Копирование исполняемого файла
echo "📦 Копирование исполняемого файла..."
cp -R "$PUBLISH_DIR/"* "$APP_MACOS_DIR/"
cp "$ICON_ICNS" "$APP_RESOURCES_DIR/"

# 4. Создание Info.plist
echo "📝 Создание Info.plist..."
cat > "$APP_BUNDLE/Contents/Info.plist" <<EOF
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN"
  "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
  <key>CFBundleExecutable</key>
  <string>$APP_NAME</string>
  <key>CFBundleIdentifier</key>
  <string>com.example.$APP_NAME</string>
  <key>CFBundleName</key>
  <string>$APP_NAME</string>
  <key>CFBundleVersion</key>
  <string>1.0</string>
  <key>CFBundlePackageType</key>
  <string>APPL</string>
  <key>CFBundleIconFile</key>
  <string>$ICON_ICNS</string>
</dict>
</plist>
EOF

echo "✅ Готово: $APP_BUNDLE создан. Запускай двойным кликом!"