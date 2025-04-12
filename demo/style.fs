module Demo.Style

open FreeSS
open FreeAct

type MyStyles = {
    root: ClassName
    header: ClassName
    appContainer: ClassName
}
let classes = makeStyles<MyStyles>()

let background = css "&"  {
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

          css "&:hover" { borderBottom "1px solid #000" }
      }
  ]
