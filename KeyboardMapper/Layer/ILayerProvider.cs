namespace Hediet.KeyboardMapper
{
    interface ILayerProvider
    {
        Layer GetLayer(SemanticKey[] pressedKeys);
    }
}