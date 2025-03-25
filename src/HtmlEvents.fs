namespace FreeAct

open Browser.Types
open FreeAct

// Event handlers
type EventHandler = Event -> unit
type PointerEventHandler = PointerEvent -> unit
type KeyboardEventHandler = KeyboardEvent -> unit
type FocusEventHandler = FocusEvent -> unit
type DragEventHandler = DragEvent -> unit
type TouchEventHandler = TouchEvent -> unit
type UIEventHandler = UIEvent -> unit

[<AutoOpen>]
module HtmlEvents =

    type CoreHtmlElementBuilder with

        // Mouse

        /// onClick event
        [<CustomOperation("onClick")>]
        member inline _.OnClick(props, handler: PointerEventHandler) =
            Prop("onClick", handler :> obj) :: props

        /// onContextMenu event
        [<CustomOperation("onContextMenu")>]
        member inline _.OnContextMenu(props, handler: PointerEventHandler) =
            Prop("onContextMenu", handler :> obj) :: props

        /// onDoubleClick event
        /// <summary>
        /// The ondblclick event occurs when the pointing device button is double-clicked over an element.
        /// </summary>
        [<CustomOperation("onDoubleClick")>]
        member inline _.OnDoubleClick(props, handler: PointerEventHandler) =
            Prop("onDoubleClick", handler :> obj) :: props

        /// onMouseDown event
        /// <summary>
        /// The onmousedown event occurs when the pointing device button is pressed over an element.
        /// </summary>
        [<CustomOperation("onMouseDown")>]
        member inline _.OnMouseDown(props, handler: PointerEventHandler) =
            Prop("onMouseDown", handler :> obj) :: props

        /// onMouseEnter event
        /// <summary>
        /// The onmouseenter event occurs when the mouse pointer is moved onto an element.
        /// </summary>
        [<CustomOperation("onMouseEnter")>]
        member inline _.OnMouseEnter(props, handler: PointerEventHandler) =
            Prop("onMouseEnter", handler :> obj) :: props

        /// onMouseLeave event
        /// <summary>
        /// The onmouseleave event occurs when the mouse pointer is moved out of an element.
        /// </summary>
        [<CustomOperation("onMouseLeave")>]
        member inline _.OnMouseLeave(props, handler: PointerEventHandler) =
            Prop("onMouseLeave", handler :> obj) :: props

        /// onMouseMove event
        /// <summary>
        /// The onmousemove event occurs when the pointer is moving while it is over an element.
        /// </summary>
        [<CustomOperation("onMouseMove")>]
        member inline _.OnMouseMove(props, handler: PointerEventHandler) =
            Prop("onMouseMove", handler :> obj) :: props

        /// onMouseOut event
        /// <summary>
        /// The onmouseout event occurs when the mouse pointer is moved out of an element.
        /// </summary>
        [<CustomOperation("onMouseOut")>]
        member inline _.OnMouseOut(props, handler: PointerEventHandler) =
            Prop("onMouseOut", handler :> obj) :: props

        /// onMouseOver event
        /// <summary>
        /// The onmouseover event occurs when the mouse pointer is moved onto an element, or onto one of its children.
        /// </summary>
        [<CustomOperation("onMouseOver")>]
        member inline _.OnMouseOver(props, handler: PointerEventHandler) =
            Prop("onMouseOver", handler :> obj) :: props

        /// onMouseUp event
        /// <summary>
        /// The onmouseup event occurs when the pointing device button is released over an element.
        /// </summary>
        [<CustomOperation("onMouseUp")>]
        member inline _.OnMouseUp(props, handler: PointerEventHandler) =
            Prop("onMouseUp", handler :> obj) :: props

        // Keyboard

        /// onKeyDown event
        /// <summary>
        /// The onkeydown event occurs when the key is pressed down.
        /// </summary>
        [<CustomOperation("onKeyDown")>]
        member inline _.OnKeyDown(props, handler: KeyboardEventHandler) =
            Prop("onKeyDown", handler :> obj) :: props

        /// onKeyPress event
        /// <summary>
        /// The onkeypress event occurs when a key is pressed down and that key normally produces a character value (use input instead).
        /// </summary>
        [<CustomOperation("onKeyPress")>]
        member inline _.OnKeyPress(props, handler: KeyboardEventHandler) =
            Prop("onKeyPress", handler :> obj) :: props

        /// onKeyUp event
        /// <summary>
        /// The onkeyup event occurs when the key is released.
        /// </summary>
        [<CustomOperation("onKeyUp")>]
        member inline _.OnKeyUp(props, handler: KeyboardEventHandler) =
            Prop("onKeyUp", handler :> obj) :: props

        // Focus

        /// onFocus event
        /// <summary>
        /// The onfocus event occurs when an element gets focus.
        /// </summary>
        [<CustomOperation("onFocus")>]
        member inline _.OnFocus(props, handler: FocusEventHandler) =
            Prop("onFocus", handler :> obj) :: props

        /// onBlur event
        /// <summary>
        /// The onblur event occurs when an element loses focus.
        /// </summary>
        [<CustomOperation("onBlur")>]
        member inline _.OnBlur(props, handler: FocusEventHandler) =
            Prop("onBlur", handler :> obj) :: props

        /// onFocusIn event
        /// <summary>
        /// The onfocusin event occurs when an element is about to get focus.
        /// </summary>
        [<CustomOperation("onFocusIn")>]
        member inline _.OnFocusIn(props, handler: FocusEventHandler) =
            Prop("onFocusIn", handler :> obj) :: props

        /// onFocusOut event
        /// <summary>
        /// The onfocusout event occurs when an element is about to lose focus.
        /// </summary>
        [<CustomOperation("onFocusOut")>]
        member inline _.OnFocusOut(props, handler: FocusEventHandler) =
            Prop("onFocusOut", handler :> obj) :: props

        // Drag

        /// onDrag event
        /// <summary>
        /// The ondrag event occurs when an element or text selection is being dragged.
        /// </summary>
        [<CustomOperation("onDrag")>]
        member inline _.OnDrag(props, handler: DragEventHandler) =
            Prop("onDrag", handler :> obj) :: props

        /// onDragEnd event
        /// <summary>
        /// The ondragend event occurs when the user has finished dragging an element or text selection.
        /// </summary>
        [<CustomOperation("onDragEnd")>]
        member inline _.OnDragEnd(props, handler: DragEventHandler) =
            Prop("onDragEnd", handler :> obj) :: props

        /// onDragEnter event
        /// <summary>
        /// The ondragenter event occurs when the dragged element enters the drop target.
        /// </summary>
        [<CustomOperation("onDragEnter")>]
        member inline _.OnDragEnter(props, handler: DragEventHandler) =
            Prop("onDragEnter", handler :> obj) :: props

        /// onDragExit event
        /// <summary>
        /// The ondragexit event occurs when the dragged element leaves the drop target.
        /// </summary>
        [<CustomOperation("onDragExit")>]
        member inline _.OnDragExit(props, handler: DragEventHandler) =
            Prop("onDragExit", handler :> obj) :: props

        /// onDragLeave event
        /// <summary>
        /// The ondragleave event occurs when the dragged element leaves the drop target.
        /// </summary>
        [<CustomOperation("onDragLeave")>]
        member inline _.OnDragLeave(props, handler: DragEventHandler) =
            Prop("onDragLeave", handler :> obj) :: props

        /// onDragOver event
        /// <summary>
        /// The ondragover event occurs when the dragged element is over the drop target.
        /// </summary>
        [<CustomOperation("onDragOver")>]
        member inline _.OnDragOver(props, handler: DragEventHandler) =
            Prop("onDragOver", handler :> obj) :: props

        /// onDragStart event
        /// <summary>
        /// The ondragstart event occurs when the user starts to drag an element or text selection.
        /// </summary>
        [<CustomOperation("onDragStart")>]
        member inline _.OnDragStart(props, handler: DragEventHandler) =
            Prop("onDragStart", handler :> obj) :: props

        /// onDrop event
        /// <summary>
        /// The ondrop event occurs when the dragged element is dropped on the drop target.
        /// </summary>
        [<CustomOperation("onDrop")>]
        member inline _.OnDrop(props, handler: DragEventHandler) =
            Prop("onDrop", handler :> obj) :: props

        // touch

        /// onTouchCancel event
        /// <summary>
        /// The ontouchcancel event occurs when the touch is interrupted.
        /// </summary>
        [<CustomOperation("onTouchCancel")>]
        member inline _.OnTouchCancel(props, handler: TouchEventHandler) =
            Prop("onTouchCancel", handler :> obj) :: props

        /// onTouchEnd event
        /// <summary>
        /// The ontouchend event occurs when a touch point is removed from the touch surface.
        /// </summary>
        [<CustomOperation("onTouchEnd")>]
        member inline _.OnTouchEnd(props, handler: TouchEventHandler) =
            Prop("onTouchEnd", handler :> obj) :: props

        /// onTouchMove event
        /// <summary>
        /// The ontouchmove event occurs when a touch point is moved along the touch surface.
        /// </summary>
        [<CustomOperation("onTouchMove")>]
        member inline _.OnTouchMove(props, handler: TouchEventHandler) =
            Prop("onTouchMove", handler :> obj) :: props

        /// onTouchStart event
        /// <summary>
        /// The ontouchstart event occurs when a touch point is placed on the touch surface.
        /// </summary>
        [<CustomOperation("onTouchStart")>]
        member inline _.OnTouchStart(props, handler: TouchEventHandler) =
            Prop("onTouchStart", handler :> obj) :: props

        // pointer

        /// onPointerDown event
        /// <summary>
        /// The onpointerdown event occurs when the pointer is pressed down over an element.
        /// </summary>
        [<CustomOperation("onPointerDown")>]
        member inline _.OnPointerDown(props, handler: PointerEventHandler) =
            Prop("onPointerDown", handler :> obj) :: props

        /// onPointerMove event
        /// <summary>
        /// The onpointermove event occurs when the pointer is moved over an element.
        /// </summary>
        [<CustomOperation("onPointerMove")>]
        member inline _.OnPointerMove(props, handler: PointerEventHandler) =
            Prop("onPointerMove", handler :> obj) :: props

        /// onPointerUp event
        /// <summary>
        /// The onpointerup event occurs when the pointer is released over an element.
        /// </summary>
        [<CustomOperation("onPointerUp")>]
        member inline _.OnPointerUp(props, handler: PointerEventHandler) =
            Prop("onPointerUp", handler :> obj) :: props

        /// onPointerCancel event
        /// <summary>
        /// The onpointercancel event occurs when the pointer is interrupted.
        /// </summary>
        [<CustomOperation("onPointerCancel")>]
        member inline _.OnPointerCancel(props, handler: PointerEventHandler) =
            Prop("onPointerCancel", handler :> obj) :: props

        /// onPointerEnter event
        /// <summary>
        /// The onpointerenter event occurs when the pointer is moved onto an element.
        /// </summary>
        [<CustomOperation("onPointerEnter")>]
        member inline _.OnPointerEnter(props, handler: PointerEventHandler) =
            Prop("onPointerEnter", handler :> obj) :: props

        /// onPointerLeave event
        /// <summary>
        /// The onpointerleave event occurs when the pointer is moved out of an element.
        /// </summary>
        [<CustomOperation("onPointerLeave")>]
        member inline _.OnPointerLeave(props, handler: PointerEventHandler) =
            Prop("onPointerLeave", handler :> obj) :: props

        /// onPointerOver event
        /// <summary>
        /// The onpointerover event occurs when the pointer is moved onto an element.
        /// </summary>
        [<CustomOperation("onPointerOver")>]
        member inline _.OnPointerOver(props, handler: PointerEventHandler) =
            Prop("onPointerOver", handler :> obj) :: props

        /// onPointerOut event
        /// <summary>
        /// The onpointerout event occurs when the pointer is moved out of an element.
        /// </summary>
        [<CustomOperation("onPointerOut")>]
        member inline _.OnPointerOut(props, handler: PointerEventHandler) =
            Prop("onPointerOut", handler :> obj) :: props

        /// onGotPointerCapture event
        /// <summary>
        /// The ongotpointercapture event occurs when an element receives pointer capture.
        /// </summary>
        [<CustomOperation("onGotPointerCapture")>]
        member inline _.OnGotPointerCapture(props, handler: PointerEventHandler) =
            Prop("onGotPointerCapture", handler :> obj) :: props

        /// onLostPointerCapture event
        /// <summary>
        /// The onlostpointercapture event occurs when an element loses pointer capture.
        /// </summary>
        [<CustomOperation("onLostPointerCapture")>]
        member inline _.OnLostPointerCapture(props, handler: PointerEventHandler) =
            Prop("onLostPointerCapture", handler :> obj) :: props

        // onchange
        [<CustomOperation("onChange")>]
        member inline _.OnChange(props, handler: EventHandler) =
            Prop("onChange", handler :> obj) :: props
