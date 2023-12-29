namespace GameOfLife.Models;

public class Renderer : IDisposable
{
  public readonly IntPtr SDLRrenderer;
  public readonly IntPtr SDLWindow;
  public readonly IntPtr SDLWindowSurface;

  public Renderer(IntPtr window, int screenIndex = 0)
  {
    SDLWindow        = window;
    SDLWindowSurface = SDL_GetWindowSurface(SDLWindow);
    SDLRrenderer     = SDL_CreateRenderer(window, screenIndex, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
  }

  public void Clear()
  {
    SDL_RenderClear(SDLRrenderer);
  }

  public void Present()
  {
    SDL_RenderPresent(SDLRrenderer);
  }

  public void SetDrawColor(SDL_Color color)
  {
    SDL_SetRenderDrawColor(SDLRrenderer, color.r, color.g, color.b, color.a);
  }

  public SDL_Color GetDrawColor()
  {
    SDL_GetRenderDrawColor(SDLRrenderer, out byte r, out byte g, out byte b, out byte a);
    return new SDL_Color { r = r, g = g, b = b, a = a };
  }

  public void DrawRect(int x, int y, int width, int height)
  {
    var rect = new SDL_Rect { x = x, y = y, w = width, h = height };
    SDL_RenderDrawRect(SDLRrenderer, ref rect);
  }

  public void DrawRect(float x, float y, float width, float height)
  {
    var rect = new SDL_FRect { x = x, y = y, w = width, h = height };
    SDL_RenderDrawRectF(SDLRrenderer, ref rect);
  }

  public void DrawRect(SDL_Rect rect)
  {
    SDL_RenderDrawRect(SDLRrenderer, ref rect);
  }

  public void DrawRect(SDL_FRect rect)
  {
    SDL_RenderDrawRectF(SDLRrenderer, ref rect);
  }

  public void FillRect(int x, int y, int width, int height)
  {
    var rect = new SDL_Rect { x = x, y = y, w = width, h = height };
    SDL_RenderDrawRect(SDLRrenderer, ref rect);
  }

  public void FillRect(float x, float y, float width, float height)
  {
    var rect = new SDL_FRect { x = x, y = y, w = width, h = height };
    SDL_RenderFillRectF(SDLRrenderer, ref rect);
  }

  public void FillRect(SDL_Rect rect)
  {
    SDL_RenderFillRect(SDLRrenderer, ref rect);
  }

  public void FillRect(SDL_FRect rect)
  {
    SDL_RenderFillRectF(SDLRrenderer, ref rect);
  }

  public void DrawLine(int x1, int y1, int x2, int y2)
  {
    SDL_RenderDrawLine(SDLRrenderer, x1, y1, x2, y2);
  }

  public void DrawLine(float x1, float y1, float x2, float y2)
  {
    SDL_RenderDrawLineF(SDLRrenderer, x1, y1, x2, y2);
  }

  public void DrawTexture(IntPtr texture)
  {
    SDL_RenderCopy(SDLRrenderer, texture, 0, 0);
  }

  public void DrawTexture(IntPtr texture, SDL_Rect dstRect)
  {
    SDL_RenderCopy(SDLRrenderer, texture, 0, ref dstRect);
  }

  public void DrawTexture(IntPtr texture, SDL_Rect dstRect, SDL_Rect srcRect)
  {
    SDL_RenderCopy(SDLRrenderer, texture, ref srcRect, ref dstRect);
  }

  public void DrawTexture(IntPtr texture, SDL_FRect dstRect)
  {
    SDL_RenderCopyF(SDLRrenderer, texture, 0, ref dstRect);
  }

  public void DrawTexture(IntPtr texture, SDL_FRect dstRect, SDL_Rect srcRect)
  {
    SDL_RenderCopyF(SDLRrenderer, texture, ref srcRect, ref dstRect);
  }

  public void DrawSurface(IntPtr surface)
  {
    SDL_BlitSurface(surface, surface, SDLWindowSurface, 0);
  }

  public void DrawSurface(IntPtr surface, SDL_Rect dstRect)
  {
    SDL_BlitSurface(surface, surface, SDLWindowSurface, ref dstRect);
  }

  public void DrawSurface(IntPtr surface, SDL_Rect dstRect, SDL_Rect srcRect)
  {
    SDL_BlitSurface(surface, ref srcRect, SDLWindowSurface, ref dstRect);
  }

  public void Dispose()
  {
    SDL_DestroyRenderer(SDLRrenderer);
  }

  public IntPtr CreateTexture(IntPtr surface)
  {
    return SDL_CreateTextureFromSurface(SDLRrenderer, surface);
  }

  public IntPtr LoadSurface(string path)
  {
    return IMG_Load(path);
  }

  public IntPtr LoadTexture(string path)
  {
    return IMG_LoadTexture(SDLRrenderer, path);
  }

  public void DestroyTexture(IntPtr texture)
  {
    SDL_DestroyTexture(texture);
  }

  public void DestroySurface(IntPtr surface)
  {
    SDL_FreeSurface(surface);
  }

  public IntPtr LoadFont(string path, int size)
  {
    return TTF_OpenFont(path, size);
  }

  public void DestroyFont(IntPtr font)
  {
    TTF_CloseFont(font);
  }

  public IntPtr RenderText(string text, IntPtr font, SDL_Color color)
  {
    return TTF_RenderText_Blended(font, text, color);
  }

  public void Screenshot(string filename, int x, int y, int width, int height)
  {
    var rect       = new SDL_Rect { x = x, y = y, w = width, h = height };
    var surfacePtr = SDL_CreateRGBSurfaceWithFormat(0, width, height, 24, SDL_PIXELFORMAT_RGB24);
    var surface    = Marshal.PtrToStructure<SDL_Surface>(surfacePtr);

    SDL_RenderReadPixels(SDLRrenderer, ref rect, SDL_PIXELFORMAT_RGB24, surface.pixels, surface.pitch);
    Marshal.StructureToPtr(surface, surfacePtr, true);

    IMG_SavePNG(surfacePtr, filename);
    SDL_FreeSurface(surfacePtr);
  }

  public void Screenshot(int x, int y, int width, int height)
  {
    string basePath = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");

    if (!Directory.Exists(basePath))
      Directory.CreateDirectory(basePath);

    string filename = Path.Combine(basePath, $"{DateTime.Now:HH:mm:ss_dd-MM-yyyy}.png");

    Screenshot(filename, x, y, width, height);
  }
}