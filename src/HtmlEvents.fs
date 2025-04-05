namespace FreeAct

open Browser.Types
open Fable.Core
open FreeAct

// Event handlers
type EventHandler = Event -> unit
type PointerEventHandler = PointerEvent -> unit
type KeyboardEventHandler = KeyboardEvent -> unit
type FocusEventHandler = FocusEvent -> unit
type DragEventHandler = DragEvent -> unit
type TouchEventHandler = TouchEvent -> unit
type UIEventHandler = UIEvent -> unit
type WheelEventHandler = WheelEvent -> unit
type ClipboardEventHandler = ClipboardEvent -> unit
type AnimationEventHandler = AnimationEvent -> unit
type TransitionEventHandler = TransitionEvent -> unit
type MediaEventHandler = Event -> unit
// TODO: specialize InputEvent
type InputEventHandler = Event -> unit

[<AutoOpen>]
module HtmlEvents =

    type CoreElementBuilder with
        // animation events

        // onAnimationStart
        /// <summary>
        /// The onanimationstart event occurs when a CSS animation has started.
        /// </summary>
        [<CustomOperation("onAnimationStart")>]
        member inline _.OnAnimationStart(props, handler: AnimationEventHandler) =
            Prop("onAnimationStart", handler :> obj) :: props

        // onAnimationEnd
        /// <summary>
        /// The onanimationend event occurs when a CSS animation has completed.
        /// </summary>
        [<CustomOperation("onAnimationEnd")>]
        member inline _.OnAnimationEnd(props, handler: AnimationEventHandler) =
            Prop("onAnimationEnd", handler :> obj) :: props

        // onAnimationIteration
        /// <summary>
        /// The onanimationiteration event occurs when a CSS animation is repeated.
        /// </summary>
        [<CustomOperation("onAnimationIteration")>]
        member inline _.OnAnimationIteration(props, handler: AnimationEventHandler) =
            Prop("onAnimationIteration", handler :> obj) :: props

        // onAnimationCancel
        /// <summary>
        /// The onanimationcancel event occurs when a CSS animation is canceled.
        /// </summary>
        [<CustomOperation("onAnimationCancel")>]
        member inline _.OnAnimationCancel(props, handler: AnimationEventHandler) =
            Prop("onAnimationCancel", handler :> obj) :: props

        // onBeforeInput
        /// <summary>
        /// The onbeforeinput event occurs before the input event.
        /// </summary>
        [<CustomOperation("onBeforeInput")>]
        member inline _.OnBeforeInput(props, handler: InputEventHandler) =
            Prop("onBeforeInput", handler :> obj) :: props

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

        // onScroll
        /// <summary>
        /// The onscroll event occurs when an element's scrollbar is being scrolled.
        /// </summary>
        [<CustomOperation("onScroll")>]
        member inline _.OnScroll(props, handler: EventHandler) =
            Prop("onScroll", handler :> obj) :: props

        // onScrollEnd
        /// <summary>
        /// The onscrollend event occurs when an element's scrollbar has been scrolled to the end.
        /// </summary>
        [<CustomOperation("onScrollEnd")>]
        member inline _.OnScrollEnd(props, handler: EventHandler) =
            Prop("onScrollEnd", handler :> obj) :: props

        // onWheel
        /// <summary>
        /// The onwheel event occurs when the mouse wheel is being rotated.
        /// </summary>
        [<CustomOperation("onWheel")>]
        member inline _.OnWheel(props, handler: WheelEventHandler) =
            Prop("onWheel", handler :> obj) :: props
        // clipboard events

        // onCopy
        /// <summary>
        /// The oncopy event occurs when the user copies the content of an element.
        /// </summary>
        [<CustomOperation("onCopy")>]
        member inline _.OnCopy(props, handler: ClipboardEventHandler) =
            Prop("onCopy", handler :> obj) :: props

        // onCut
        /// <summary>
        /// The oncut event occurs when the user cuts the content of an element.
        /// </summary>
        [<CustomOperation("onCut")>]
        member inline _.OnCut(props, handler: ClipboardEventHandler) =
            Prop("onCut", handler :> obj) :: props

        // onPaste
        /// <summary>
        /// The onpaste event occurs when the user pastes some content in an element.
        /// </summary>
        [<CustomOperation("onPaste")>]
        member inline _.OnPaste(props, handler: ClipboardEventHandler) =
            Prop("onPaste", handler :> obj) :: props

        // transition events

        // onTransitionEnd
        /// <summary>
        /// The ontransitionend event occurs when a CSS transition has completed.
        /// </summary>
        [<CustomOperation("onTransitionEnd")>]
        member inline _.OnTransitionEnd(props, handler: TransitionEventHandler) =
            Prop("onTransitionEnd", handler :> obj) :: props

        // onTransitionCancel
        /// <summary>
        /// The ontransitioncancel event occurs when a CSS transition is canceled.
        /// </summary>
        [<CustomOperation("onTransitionCancel")>]
        member inline _.OnTransitionCancel(props, handler: TransitionEventHandler) =
            Prop("onTransitionCancel", handler :> obj) :: props

        // onTransitionRun
        /// <summary>
        /// The ontransitionrun event occurs when a CSS transition starts.
        /// </summary>
        [<CustomOperation("onTransitionRun")>]
        member inline _.OnTransitionRun(props, handler: TransitionEventHandler) =
            Prop("onTransitionRun", handler :> obj) :: props

        // onTransitionStart
        /// <summary>
        /// The ontransitionstart event occurs when a CSS transition starts.
        /// </summary>
        [<CustomOperation("onTransitionStart")>]
        member inline _.OnTransitionStart(props, handler: TransitionEventHandler) =
            Prop("onTransitionStart", handler :> obj) :: props

        // onInput
        /// <summary>
        ///
        /// </summary>
        [<CustomOperation("onInput")>]
        member inline _.OnInput(props, handler: InputEventHandler) =
            Prop("onInput", handler :> obj) :: props

    type HtmlElementBuilder with

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

        // onChange
        /// <summary>
        /// The onchange event occurs when the value of an element has been changed.
        /// </summary>
        [<CustomOperation("onChange")>]
        member inline _.OnChange(props, handler: EventHandler) =
            Prop("onChange", handler :> obj) :: props

    type FormElement with
        [<CustomOperation("onSubmit")>]
        member inline _.OnSubmit(props, value: EventHandler) =
            Prop("onSubmit", value :> obj) :: props

        [<CustomOperation("onReset")>]
        member inline _.OnReset(props, value: EventHandler) = Prop("onReset", value :> obj) :: props

    type MediaElementBuilder with
        /// Fired when media can start playing
        [<CustomOperation("onCanPlay")>]
        member inline _.OnCanPlay(props, handler: MediaEventHandler) =
            Prop("onCanPlay", handler :> obj) :: props

        /// Fired when media has finished loading
        [<CustomOperation("onLoadedData")>]
        member inline _.OnLoadedData(props, handler: MediaEventHandler) =
            Prop("onLoadedData", handler :> obj) :: props

        /// Fired when media playback ends
        [<CustomOperation("onEnded")>]
        member inline _.OnEnded(props, handler: MediaEventHandler) =
            Prop("onEnded", handler :> obj) :: props

    type DetailsElementBuilder with
        /// Fired when the details element is toggled
        [<CustomOperation("onToggle")>]
        member inline _.OnToggle(props, handler: EventHandler) =
            Prop("onToggle", handler :> obj) :: props

    type DialogElementBuilder with
        /// Fired when the dialog is closed
        [<CustomOperation("onClose")>]
        member inline _.OnClose(props, handler: EventHandler) =
            Prop("onClose", handler :> obj) :: props

        /// Fired when the dialog is cancelled (e.g., by pressing Escape)
        [<CustomOperation("onCancel")>]
        member inline _.OnCancel(props, handler: EventHandler) =
            Prop("onCancel", handler :> obj) :: props
