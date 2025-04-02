using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogueNode : Node {
    public Dialogue dialogue;

    [Input] public Node prevNode;

    [Output] public Node nextNode;

}