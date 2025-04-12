module Demo.Style

open FreeSS
open FreeAct

type MyStyles = {
    root: ClassName
    appContainer: ClassName
}
let classes = makeStyles<MyStyles>()

let background = mix  {
    backgroundColor "#f0f0f0"
    margin 4
} 

fss [

      css "#root" { fontFamily "Arial, sans-serif" }
      css classes.appContainer {
          flex
          background

          css "nested" { color "#f0f0f0" }
      }

      css "header" {
          flex
          background

          css "ul" {
            listStyleType ListStyle.None
          }

          css "a" { 

            textDecoration TextDecoration.None

            css "&:hover" {
                textDecoration TextDecoration.Underline
            }
            
          }
      }
  ]
