module Demo.Style

open FreeSS


let classes =   fss [

      css "#root" { fontFamily "Arial, sans-serif" }
      css ".app-container" {
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
