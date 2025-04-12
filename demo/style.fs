module Demo.Style

open FreeSS

type MyStyles = {
    root: ClassName
    header: ClassName
    appContainer: ClassName
}
let classes = makeStyles<MyStyles>()


fss [

      css "#root" { fontFamily "Arial, sans-serif" }
      css classes.appContainer {
          flex
          backgroundColor "#f0f0f0"

          css "nested" { color "#f0f0f0" }
      }

      css "header" {
          flex
          backgroundColor "#f0f0f0"

          css "&:hover" { borderBottom "1px solid #000" }
      }
  ]
