namespace ConsoleApplicationNeoTest
{
    interface ILayerProvider
    {
        Layer GetLayer(SemanticKey[] pressedKeys);
    }
}