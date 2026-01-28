SET(SOURCES screens/ui_Home.c
    ui.c
    components/ui_comp_hook.c
    ui_helpers.c
    images/ui_img_842386787.c
    images/ui_img_794451024.c
    images/ui_img_1133614182.c
    images/ui_img_1763518677.c
    fonts/ui_font_FontAwesome.c)

add_library(ui ${SOURCES})
