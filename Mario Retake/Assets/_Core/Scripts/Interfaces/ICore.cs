using UnityEngine;


public interface ICore : ICoreInteractor, ICoreModifier
{

}

public interface ICoreInteractor
{
    void OnInteractBegin(InteractionResult res);
    void OnInteractEnd(InteractionResult res);
}

public interface ICoreModifier
{
}


public class InteractionResult
{

}